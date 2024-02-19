using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{
    // Bu farklı şekillerde yazıla ama dikkat etmemiz gereken nokta dönen property isimleri aynı olmalı ki client ler bunu rahat bir şekilde algılaya bilsin

    // Örnek olarak geriye bir şey dönmiyeceğimiz zamanda aşağıdaki kod kullanıla bilir
    //public class CustomNoContentResponseDto
    //{ 
    //    [JsonIgnore]
    //    public int StatusCode { get; set; }
    //    public List<String> Errors { get; set; }
    //    public static CustomNoContentResponseDto Success(int statusCode)
    //    {
    //        return new CustomNoContentResponseDto { StatusCode = statusCode };
    //    }
    //    public static CustomNoContentResponseDto Fail(int statusCode, List<string> errors)
    //    {
    //        return new CustomNoContentResponseDto { StatusCode = statusCode, Errors = errors };
    //    }
    //    public static CustomNoContentResponseDto Fail(int statusCode, string error)
    //    {
    //        return new CustomNoContentResponseDto { StatusCode = statusCode, Errors = new List<string> { error } };
    //    }
    //}

    public class CustomResponseDto<T>
    {
        // API lar da end point leri yazarken tek bir model dönmek için var işlem başarılı da olsa başarısız da olsa geriye döneceğimiz modelin tek olmalı
        // işlem başarılı da olsa başarısız da olsa tek model döneceğiz bu sayede front end de yoksa front end de 2 fark lı model eklenmek zorunda olacak
        public T Data { get; set; }

        [JsonIgnore] // Status codu dönmemize gerek yok çünkü client zaten isteklerin status kodlarını alabiliyor bu yüzden JsonIgnore diyoruz
        public int StatusCode { get; set; }
        public List<String> Errors { get; set; }

        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
