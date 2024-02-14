using CQRS.BankAPI.Domain.Abstractions;

namespace CQRS.BankAPI.Domain.Entities.Users;

public static class UserErrors
{
    public static Error NotFound = new(
        "User.NotFound",
        "El usuario especificado no fue encontrado");

    public static Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "Las credenciales proporcionadas son inválidas");

    public static Error InvalidEmail = new(
        "User.InvalidEmail",
        "El correo electrónico proporcionado es inválido");

    public static Error AlreadyExists = new(
        "User.AlreadyExists",
        "El usuario especificado ya existe");

    public static Error DniAlreadyExists = new(
        "User.DniAlreadyExists",
        "El DNI especificado ya existe");



}