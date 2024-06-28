namespace ProjectAPBD.Errors;

public class NotFoundError(string message) : Exception(message);

public class AmountError(string message) : Exception(message);

public class DateError(string message) : Exception(message);

public class ValidContractError(string message) : Exception(message);

public class TypeError(string message) : Exception(message);

public class BadRequestError(string message) : Exception(message);

