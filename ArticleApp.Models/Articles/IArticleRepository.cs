using Dul.Domain.Common;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// Repository Interface
    /// 
    /// CRUD 관련 메서드 이름을 지을 때에는 
    /// Add, Get, Update(Edit), Remove(Delete) 
    /// 등의 단어를 많이 사용한다
    /// 이러한 단어를 접두사 또는 접미사로 사용하는 것은 권장이지 필수는 아니다.
    /// </summary>
    public interface IArticleRepository
    {
        Task<Article> AddArticleAsync(Article article);     //입력
        Task<List<Article>> GetArticlesAsync();             //출력
        Task<Article> GetArticleByIdAsync(int id);          //상세
        Task<Article> EditArticleAsync(Article article);    //수정
        Task DeleteArticleAsync(int id);                     //삭제

        Task<PagingResult<Article>> GetAllAsync(
            int pageIndex, int pageSize);                   //페이징
    }
}
