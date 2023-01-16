using Microsoft.Extensions.Options;
using SalveApp.Clinics.Core.Config;

namespace SalveApp.Clinics.Web.IOC;

public class CacheServicesConfigurationValidation : IValidateOptions<CacheServicesConfig>
{
    public ValidateOptionsResult Validate(string configKey, CacheServicesConfig options)
    {
        switch (configKey)
        {
            case CacheServicesConfig.CacheServices:
            {
                var result = ValidateClinicApiConfig(options);
                if (result.Failed) return result;
                break;
            }
            default:
                return ValidateOptionsResult.Skip;
        }

        return ValidateOptionsResult.Success;
    }

    private static ValidateOptionsResult ValidateClinicApiConfig(CacheServicesConfig options)
    {
        return options.MinsToCache < 10
            ? ValidateOptionsResult.Fail("The Clinic API cache must be at least 10 minutes.")
            : ValidateOptionsResult.Success;
    }
}