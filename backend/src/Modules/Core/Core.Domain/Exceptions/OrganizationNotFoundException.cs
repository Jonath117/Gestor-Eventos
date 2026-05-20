namespace Core.Domain.Exceptions;

public class OrganizationNotFoundException(Guid organizationId) : Exception($"Organization with ID {organizationId} was not found.");