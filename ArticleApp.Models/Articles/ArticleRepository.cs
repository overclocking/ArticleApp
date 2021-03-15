using Dul.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// Repository Class: ADO.NET 또는 Dapper 또는 Entity Framework Core
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleAppDbContext _context;
        public ArticleRepository(ArticleAppDbContext context)
        {
            this._context = context;
        }

        // 입력
        public async Task<Article> AddArticleAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        // 출력
        public async Task<List<Article>> GetArticlesAsync()
        {
            return await _context.Articles.OrderByDescending(a => a.Id).ToListAsync();
        }

        // 상세
        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _context.Articles.Where(a => a.Id == id).SingleOrDefaultAsync();
            //return await _context.Articles.FindAsync(id);
        }

        // 수정
        public async Task<Article> EditArticleAsync(Article article)
        {
            _context.Entry(article).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return article;
        }

        // 삭제
        public async Task DeleteArticleAsync(int id)
        {
            //var model = await _context.Articles.FindAsync(id);
            var model = await _context.Articles.Where(m => m.Id == id).SingleOrDefaultAsync();
            if (model != null)
            {
                _context.Articles.Remove(model);
                await _context.SaveChangesAsync();
            }

        }

        // 페이징
        public async Task<PagingResult<Article>> GetAllAsync(int pageIndex, int pageSize)
        {
            var totalRecords = await _context.Articles.CountAsync();
            var articles = await _context.Articles.OrderByDescending(a => a.Id)
                                                  .Skip(pageIndex * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
            return new PagingResult<Article>(articles, totalRecords);
        }

     
       
    }
}
