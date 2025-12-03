using Blog.Data.Entityes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Blog.Controllers
{
    public class UserController : Controller
    {
        /*
        public async Task RegisterUser(string email, string password)
        {
            // 1. Create the Domain Event
            var user = new User(email, password);
            var domainEvent = new UserRegisteredEvent(user.Id, user.Email);

            // 2. Open a Transaction
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                // 3. Save the User to the Users Table
                dbContext.Users.Add(user);

                // 4. Serialize the Event and Save to Outbox Table
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type = nameof(UserRegisteredEvent),
                    Content = JsonSerializer.Serialize(domainEvent),
                    OccurredOn = DateTime.UtcNow,
                    ProcessedOn = null // Null means it hasn't been handled yet
                };

                dbContext.OutboxMessages.Add(outboxMessage);

                // 5. Commit BOTH changes atomically
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        */

        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
