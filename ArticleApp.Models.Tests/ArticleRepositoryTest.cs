// Install-Package Microsoft.EntityFrameworkCore.SqlServer
// Install-Package Microsoft.EntityFrameworkCore.InMemory
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleApp.Models.Tests
{
    [TestClass]
    public class ArticleRepositoryTest
    {
        [TestMethod]
        public async Task ArticleRepositoryAllMethodTest()
        {
            // DbContextOption<T> Object Creation
            var options = new DbContextOptionsBuilder<ArticleAppDbContext>()
                .UseInMemoryDatabase(databaseName: "ArticleApp").Options;   //트렌젝션은 InMemory에서 작동되지 않음
                                                                            //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=ArticleApp;integrated security=true").Options;   //실제 sql서버에 올라가는거라 재실행시 에러뜸, 값이 들어있기때문


            // AddAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Repository Object Creation
                var repository = new ArticleRepository(context);
                var model = new Article { Title = "[1] 게시판 시작", Created = DateTime.Now };
                await repository.AddArticleAsync(model);
                await context.SaveChangesAsync();
            }
            using (var context = new ArticleAppDbContext(options))
            { //InMemory는 휘발성이기에 여러번 태스트해도 매번 처음실행하는것처럼 됨
                Assert.AreEqual(1, await context.Articles.CountAsync());
                var model = await context.Articles.Where(m => m.Id == 1).SingleOrDefaultAsync();
                Assert.AreEqual("[1] 게시판 시작", model?.Title);
            }

            // GetAllAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // InMemory 테스트용 

                // Repository Object Creation
                var repository = new ArticleRepository(context);
                var model = new Article { Title = "[2] 게시판 가동", Created = DateTime.Now };
                // await repository.AddArticleAsync(model);  AddArticleAsync 테스트는 위에서 했으니 다른방법으로, 이거 써도 되긴함
                context.Articles.Add(model);
                await context.SaveChangesAsync(); //[1]
                context.Articles.Add(new Article { Title = "[3] 게시판 중지", Created = DateTime.Now });
                await context.SaveChangesAsync(); //[2]
                                                  // 현제까지 총 3개의 게시글 


                /* 트랜젝션
                using (var transaction = context.Database.BeginTransaction())   //트렌젝션은 InMemory에서 작동되지 않음
                {
                    try
                    {
                        // Repository Object Creation
                        var repository = new ArticleRepository(context);
                        var model = new Article { Title = "[2] 게시판 가동", Created = DateTime.Now };
                        // await repository.AddArticleAsync(model);  AddArticleAsync 테스트는 위에서 했으니 다른방법으로, 이거 써도 되긴함
                        context.Articles.Add(model);
                        await context.SaveChangesAsync(); //[1]
                        context.Articles.Add(new Article { Title = "[3] 게시판 중지", Created = DateTime.Now });
                        await context.SaveChangesAsync(); //[2]
                        // 현제까지 총 3개의 게시글 

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        //empty
                    }
                }
                */
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var models = await repository.GetArticlesAsync();   // 전체레코드 읽어오기, 3개여야함
                Assert.AreEqual(3, models.Count);

            }

            // GetByIdAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = await repository.GetArticleByIdAsync(2);
                Assert.IsTrue(model.Title.Contains("가동"));
                //Assert.IsTrue(model.Title.Contains("중지")); 에러뜨는거 확인
                Assert.AreEqual("[2] 게시판 가동", model.Title);

            }

            // GetEditAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = await repository.GetArticleByIdAsync(2);
                model.Title = "[2] 게시판 바쁨";
                await repository.EditArticleAsync(model);
                await context.SaveChangesAsync();

                Assert.AreEqual("[2] 게시판 바쁨",
                    (await context.Articles.Where(m => m.Id == 2).SingleOrDefaultAsync()).Title);

            }

            // GetDeleteAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                await repository.DeleteArticleAsync(2);
                await context.SaveChangesAsync();

                Assert.AreEqual(2, await context.Articles.CountAsync()); // 3->2, 2개 있어야함
                Assert.IsNull(await repository.GetArticleByIdAsync(2)); // 2번 게시글 삭제됬으니 널값이 와야함

            }

            // PagingAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                int pageIndex = 0;
                int pageSize = 1;

                var repository = new ArticleRepository(context);
                var models = await repository.GetAllAsync(pageIndex, pageSize);

                Assert.AreEqual("[3] 게시판 중지", models.Records.FirstOrDefault().Title);
                Assert.AreEqual(2, models.TotalRecords);
            }
        }
    }
}
