using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductsSample.Infrastructure.Converters;

public sealed class UlidEFConverter(ConverterMappingHints mappingHints = null!)
    : ValueConverter<Ulid, string>(
            convertToProviderExpression: x => x.ToString(),
            convertFromProviderExpression: x => Ulid.Parse(x),
            mappingHints: defaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints defaultHints = new(size: 26);

    public UlidEFConverter() : this(null!)
    { }
}