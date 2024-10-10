namespace ProductsSample.Abstractions.Primitives;

public abstract class ValueObject<T>() : NotifiableObject<T>() where T : ValueObject<T>;