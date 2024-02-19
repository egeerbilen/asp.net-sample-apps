using AutoMapper;
using Core.DTOs;
using Core.Model;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Service.Services
{
    // Best practice budur
    public class GenericService<Entity, Dto> : IGenericService<Entity, Dto> where Entity : BaseEntity where Dto : class
    {
        private readonly IGenericRepository<Entity> _repository;
        protected readonly IUnitOfWork _unitOfWork; // veri tabanına yansıtmak için bu interface var
        // önce cache alıyor ve sonrasında atıyoruz her hangi birinde hata varsa veri tabanına atmıyoruz
        protected readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomResponseDto<Dto>> AddAsync(Dto dto)
        {
            Entity newEntity = _mapper.Map<Entity>(dto);
            await _repository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync(); // Unitofwork üzerinden save change metodunu çağırıyoruz
            var newDto = _mapper.Map<Dto>(dto);

            return CustomResponseDto<Dto>.Success(StatusCodes.Status200OK, newDto);
        }

        public async Task<CustomResponseDto<IEnumerable<Dto>>> AddRangeAsync(IEnumerable<Dto> dtos) // servis tarafında liste tipinde bir arama sıralama filtreleme gibi işlemler olabileceği için Ienumerable kullandık
        {
            var newEntities = _mapper.Map<IEnumerable<Entity>>(dtos);
            await _repository.AddRangeAsync(newEntities);
            await _unitOfWork.CommitAsync();
            var newDtos = _mapper.Map<IEnumerable<Dto>>(dtos);

            return CustomResponseDto<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, newDtos);
        }

        public async Task<CustomResponseDto<bool>> AnyAsync(Expression<Func<Entity, bool>> expression)
        {
            var anyEntity = await _repository.AnyAsync(expression);
            return CustomResponseDto<bool>.Success(StatusCodes.Status200OK, anyEntity);
        }

        public async Task<CustomResponseDto<IEnumerable<Dto>>> GetAllAsync()
        {
            var entities = await _repository.GetAll().ToListAsync();// ToListAsync, Entity Framework'te bir sorgunun sonuçlarını bir List koleksiyonuna dönüştürmek için kullanılan bir metottur. Bu metot, sorgu sonuçlarını asenkron olarak alır ve bir List koleksiyonuna dönüştürür.
            var dtos = _mapper.Map<IEnumerable<Dto>>(entities);
            return CustomResponseDto<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<CustomResponseDto<Dto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            //if (hasProduct == null)
            //{

            //    throw new NotFoundExcepiton($"{typeof(T).Name}({id}) not found"); // -> response de Data ve Error büyük harfle yazılmış bu sıkıntı ola bilir çünkü diğer yerlerde küçük harf ile yazdık
            //    return default; // -> data ve error küçük default dediğmizde
            //    // bunu her yere yazmak doğru değil bunu bir yerden yöneksek daha güzel olur
            //    // kullanıcıya sms atsın mail atsın gibi durumlarda ayrı bir filter yazmak daha iyi olacaktır
            //    // eğer id yoksa bu filterdan response dönsün ek bussines kodu çalıştırcaksak da filter içinde çalışsın
            //}

            var dto = _mapper.Map<Dto>(entity);
            return CustomResponseDto<Dto>.Success(StatusCodes.Status200OK, dto);
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveRangeAsync(IEnumerable<int> ids)
        {
            var entities = await _repository.Where(x => ids.Contains(x.Id)).ToListAsync();
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(Dto dto)
        {
            var entity = _mapper.Map<Entity>(dto);
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponseDto<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression)
        {
            var entities = await _repository.Where(expression).ToListAsync();
            var dtos = _mapper.Map<IEnumerable<Dto>>(entities);
            return CustomResponseDto<IEnumerable<Dto>>.Success(StatusCodes.Status200OK, dtos);
        }
    }
}
