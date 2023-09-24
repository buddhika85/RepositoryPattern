using Microsoft.EntityFrameworkCore;
using Models;

namespace CustomerAPI_RepositoryPattern.Data
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public SqlCustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.CustomerGuid == customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));
            await _context.Customers.AddAsync(customer);
        }

        public void DeleteCommand(Customer customer)
        {
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));
            _context.Customers.Remove(customer);
        }
    }
}
