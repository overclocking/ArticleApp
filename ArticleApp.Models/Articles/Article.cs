using Dul.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace ArticleApp.Models
{
    /// <summary>
    /// 게시판 (Article)모델 클래스: Articles 테이블과 1:1로 매핑
    /// </summary>
    public class Article : AuditableBase
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 제목, 빈값 불가, 255자 한정
        /// </summary>
        [Required(ErrorMessage = "[ ! ] 제목을 입력하세요!"), MaxLength(255)]
        public string Title { get; set; }
        
        /// <summary>
        /// 내용
        /// </summary>
        [Required(ErrorMessage = "[ ! ] 내용을 입력하세요!")]
        public string Content { get; set; }

        /// <summary>
        /// 공지
        /// </summary>
        public bool IsPinned { get; set; } = false;
    }
}
