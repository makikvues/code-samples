namespace DemoApp.Implementation.Services
{
    using DemoApp.Implementation.Models;
    using DemoApp.Interfaces;

    public class InvoiceService : IService<Invoice>
    {
        // TODO
        // real implementation
        private readonly IRepository<Invoice> _repository;
        private readonly ILogger _logger;

        public InvoiceService(IRepository<Invoice> repository, ILogger logger)
        {
            this._logger = logger;
            this._repository = repository;
        }

        public Invoice Create(Invoice invoice)
        {
            this._logger.Log("custom message");

            var saved = this._repository.Save(invoice);

            return saved;
        }
    }
}
