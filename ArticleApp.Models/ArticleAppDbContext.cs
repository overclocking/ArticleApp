using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ArticleApp.Models
{
    /// <summary>
    /// DbContext Class
    /// </summary>
    public class ArticleAppDbContext : DbContext
    {
        /// Install-Package Microsoft.EntityFrameworkCore.SqlServer
        /// Install-Package Microsoft.EntityFrameworkCore.InMemory
        /// Install-Package System.Configuration.ConfigurationManager
        public ArticleAppDbContext()
        {
            // Empty
        }
        public ArticleAppDbContext(DbContextOptions<ArticleAppDbContext> options) : base(options)
        {
            // 공식같은 코드
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {   // .netCore에서는 없어도 됨
            // 닷넷 프레임워크 기반에서 호출되는 코드 영역:
            // App.config 또는 Web.config의 연결 문자열 사용
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////[A] Articles 테이블의 Created 열은 자동으로 GetDate() 제약 조건을 부여하기
            modelBuilder.Entity<Article>().Property(m => m.Created).HasDefaultValueSql("GetDate()");
        }
        //[!] ArticleApp 관련 모든 테이블 참조
        public DbSet<Article> Articles { get; set; }
    }
}
