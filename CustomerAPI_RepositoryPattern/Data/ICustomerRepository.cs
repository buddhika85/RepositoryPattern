using Models;

namespace CustomerAPI_RepositoryPattern.Data
{
    public interface ICustomerRepository
    {
        Task SaveChangesAsync();
        Task<Customer?> GetCustomerByIdAsync(string customerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task CreateCustomerAsync(Customer customer);
        // update

        void DeleteCustomer(Customer customer);
    }
}
