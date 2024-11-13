using ContactsAPI.Application.Features.AllPerson.Commands;
using ContactsAPI.Application.Features;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.AllPerson.Handlers
{
    public class GetAllPersonQueryHandler : IRequestHandler<GetAllPersonQuery, CommandResult>
    {
        private readonly ICrudRepository<Person> _personRepository;
        public GetAllPersonQueryHandler(ICrudRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<CommandResult> Handle(GetAllPersonQuery request, CancellationToken cancellationToken)
        {
            var people = await _personRepository.GetAllAsync(cancellationToken);

            return new CommandResult(CommandStatus.Success, value: people);
        }
    }
}
