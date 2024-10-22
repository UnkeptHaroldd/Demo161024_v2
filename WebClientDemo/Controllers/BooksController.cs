using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APIDemo161024.DatabaseConnect;
using APIDemo161024.Models;
using APIDemo161024.Repositories;
using System.Net.Http.Headers;
using System.Text.Json;
using APIDemo161024.DTOS;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using APIDemo161024.Helpers;

namespace WebClientDemo.Controllers
{
    public class BooksController : Controller
    {
        private readonly HttpClient client = null;

        public BooksController()
        {
            client = new HttpClient();
            var contentType =
                new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        // GET: Books
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not available
                return Unauthorized(); // Or redirect to a login page, etc.
            }

            using (var client = new HttpClient())
            {
                // Add the authorization header with the token from the cookie
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(APIEndPoint.GetBookList);

                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<BookModel> listBooks = JsonSerializer.Deserialize<List<BookModel>>(strData, options);

                return View(listBooks);
            }
        }


        // GET: Books/Create
        [Authorize(Roles = AppRole.Admin)]
        public IActionResult Create()
        {

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,Quantity")] BookModel book)
        {
            // Retrieve the token from the cookie
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not available
                return Unauthorized(); // Or redirect to a login page, etc.
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            // Serialize the Book data
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var bookJson = JsonSerializer.Serialize(book, options);

            // Prepare the content to send in the POST request
            var content = new StringContent(bookJson, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                // Add the authorization header with the token from the cookie
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send a POST request to the API endpoint
                HttpResponseMessage response = await client.PostAsync(APIEndPoint.AddBook, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                // If something went wrong, return the view with the Book data
                string strData = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Error creating Book: " + strData);

                return View(book);
            }
        }

        // GET: Books/Edit/5
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the token from the cookie
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not available
                return Unauthorized(); // Or redirect to a login page, etc.
            }

            using (var client = new HttpClient())
            {
                // Add the authorization header with the token from the cookie
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(APIEndPoint.GetBookByID + id);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the case where the request was not successful
                    return NotFound();
                }

                string strData = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                BookModel book = JsonSerializer.Deserialize<BookModel>(strData, options);

                if (book == null)
                {
                    return NotFound();
                }

                return View(book);
            }
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,Quantity")] BookModel book)
        {
            if (id != book.Id)
            {
                return BadRequest(); // Ensure the ID from route matches the Book's ID
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            // Retrieve the token from the cookie
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
   
                return Unauthorized(); 
            }

            // Serialize the Book data
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var bookJson = JsonSerializer.Serialize(book, options);

            // Prepare the content to send in the PUT request
            var content = new StringContent(bookJson, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                // Add the authorization header with the token from the cookie
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send a PUT request to the API endpoint
                HttpResponseMessage response = await client.PutAsync(APIEndPoint.EditBook + book.Id, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                // If something went wrong, return the view with the Book data
                string strData = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, "Error updating Book: " + strData);

                return View(book);
            }
        }

        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the token from the cookie
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not available
                return Unauthorized(); // Or redirect to a login page, etc.
            }

            using (var client = new HttpClient())
            {
                // Add the authorization header with the token from the cookie
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(APIEndPoint.GetBookByID + id);

                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                string strData = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                BookModel book = JsonSerializer.Deserialize<BookModel>(strData, options);
                if (book == null)
                {
                    return NotFound();
                }

                return View(book);
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve the token from the cookie
            string token = HttpContext.Request.Cookies["AuthToken"];

            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where the token is not available
                return Unauthorized(); // Or redirect to a login page, etc.
            }

            using (var client = new HttpClient())
            {
                // Add the authorization header with the token from the cookie
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.DeleteAsync(APIEndPoint.DeleteBook + id);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the case where the delete request was not successful
                    string strData = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "Error deleting Book: " + strData);
                    return RedirectToAction(nameof(Delete), new { id });
                }

                return RedirectToAction(nameof(Index));
            }
        }

    }
}
