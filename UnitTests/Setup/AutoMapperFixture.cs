using AutoMapper;
using NUnit.Framework;

public class AutoMapperFixture
{
    public static IMapper CreateMapper(params Profile[] profiles)
    {
        var cfg = new MapperConfiguration(c =>
        {
            if (profiles is { Length: > 0 })
            {
                foreach (var p in profiles) c.AddProfile(p);
            }
        });

        cfg.AssertConfigurationIsValid();
        return cfg.CreateMapper();
    }
}