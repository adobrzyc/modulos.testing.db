// ReSharper disable ClassNeverInstantiated.Global

namespace SimpleDomain.Tests.Blocks
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Modulos.Testing;

    public sealed class InitializeIoc : IBlock, IServiceCollection
    {
        public readonly ContainerBuilder Autofac;
        private readonly IServiceCollection internalCollection;

        public InitializeIoc()
        {
            internalCollection = new ServiceCollection();
            Autofac = new ContainerBuilder();
        }


        Task<BlockExecutionResult> IBlock.Execute(ITestEnvironment testEnv)
        {
            var sc = new ServiceCollection();
            foreach (var serviceDescriptor in internalCollection) sc.Add(serviceDescriptor);

            Autofac.Populate(sc);

            var sp = new AutofacServiceProvider(Autofac.Build());

            testEnv.SetServiceProvider(sp);

            return Task.FromResult(BlockExecutionResult.EmptyContinue);
        }

        Task IBlock.ExecuteAtTheEnd(ITestEnvironment testEnv)
        {
            ((IServiceCollection)this).Clear();
            return Task.CompletedTask;
        }


        #region IServiceCollection

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
        {
            return internalCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)internalCollection).GetEnumerator();
        }

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
        {
            internalCollection.Add(item);
        }

        void ICollection<ServiceDescriptor>.Clear()
        {
            internalCollection.Clear();
        }

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
        {
            return internalCollection.Contains(item);
        }

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            internalCollection.CopyTo(array, arrayIndex);
        }

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
        {
            return internalCollection.Remove(item);
        }

        int ICollection<ServiceDescriptor>.Count => internalCollection.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => internalCollection.IsReadOnly;

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
        {
            return internalCollection.IndexOf(item);
        }

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
        {
            internalCollection.Insert(index, item);
        }

        void IList<ServiceDescriptor>.RemoveAt(int index)
        {
            internalCollection.RemoveAt(index);
        }

        ServiceDescriptor IList<ServiceDescriptor>.this[int index]
        {
            get => internalCollection[index];
            set => internalCollection[index] = value;
        }

        #endregion
    }
}