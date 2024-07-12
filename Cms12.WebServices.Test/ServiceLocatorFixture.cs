using EPiServer;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Cms12.WebServices.Test;

/// <summary>
/// Mock service locator for tests
/// </summary>
public class ServiceLocatorFixture : IDisposable
{
    /// <summary>
    /// Add items to DI on constructor
    /// </summary>
    public ServiceLocatorFixture()
    {
        this.Providing<IContentRepository>();
        this.Providing<IUrlResolver>();
    }

    /// <summary>
    /// Encapsulated service provider
    /// </summary>
    private IServiceProvider _serviceProvider;

    /// <summary>
    /// Public accessor for service locator
    /// </summary>
    public IServiceProvider ServiceProvider
    {
        get
        {
            // If it already exists, just return it            
            if (this._serviceProvider != null)
            {
                return this._serviceProvider;
            }
            // Create new service provider
            this._serviceProvider = Substitute.For<IServiceProvider>();
            ServiceLocator.SetServiceProvider(this._serviceProvider);
            return this._serviceProvider;
        }
    }

    /// <summary>
    /// Add item to service locator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Providing<T>() where T : class
    {
        var obj = Substitute.For<T>();
        this.ServiceProvider.GetService<T>().Returns(obj);
        this.ServiceProvider.GetRequiredService<T>().Returns(obj);
        return obj;
    }

    /// <summary>
    /// Add item to service locator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <returns></returns>
    public T Providing<T>(T instance) where T : class
    {
        this.ServiceProvider.GetService<T>().Returns(instance);
        this.ServiceProvider.GetRequiredService<T>().Returns(instance);
        return instance;
    }
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    { }
}