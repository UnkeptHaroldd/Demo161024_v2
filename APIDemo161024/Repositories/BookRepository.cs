using APIDemo161024.DatabaseConnect;
using APIDemo161024.DTOS;
using APIDemo161024.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace APIDemo161024.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public BookRepository(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BookRepository()
        {

        }

        public async Task<int> AddBookAsync(BookModel model)
        {
            var newBook = _mapper.Map<Book>(model);
            _context.Book!.Add(newBook);
            await _context.SaveChangesAsync();

            return newBook.Id;
        }

        public async Task DeleteBookAsync(int id)
        {
            var deleteBook = _context.Book!.SingleOrDefault(b => b.Id == id);
            if (deleteBook != null)
            {
                _context.Book!.Remove(deleteBook);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            var books = await _context.Book.ToListAsync();
            return _mapper.Map<List<BookModel>>(books);
        }

        public async Task<BookModel> GetBookAsync(int id)
        {
            var book = await _context.Book!.FindAsync(id);
            return _mapper.Map<BookModel>(book);
        }

        public async Task UpdateBookAsync(int id, BookModel model)
        {
            if (id == model.Id)
            {
                var updateBook = _mapper.Map<Book>(model);
                _context.Book!.Update(updateBook);
                await _context.SaveChangesAsync();
            }
        }
    }
}
