using Krypton.Support.ComponentModel.Contracts;
using Krypton.Support.ComponentModel.Internal;
using System.Collections.Generic;

namespace Krypton.Support
{
    public abstract class KryptonComponent<T> : IKryptonComponent where T : IKryptonComponent, new()
    {
        /// <summary>
        /// Returns requested <see cref="IKryptonComponent"/> implemented component.
        /// Create the new object if requested component does not exists.
        /// </summary>
        public static T Instance;

        public bool IsBuilded { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        static KryptonComponent()
        {
            if (Instance == null)
            {
                Instance = StaticCore.Add(new T());
                var method = Instance.GetType().GetMethod("Build");
                if (method != null)
                {
                    method.Invoke(Instance, null);
                }
            }
        }

        /// <summary>
        /// Returns requested <see cref="IKryptonComponent"/> implemented component(internal variant)
        /// </summary>
        /// <typeparam name="TReq">IKryptonComponent</typeparam>
        /// <returns></returns>
        protected TReq GetComponent<TReq>() where TReq : IKryptonComponent, new()
            => StaticCore.Get<TReq>();

        /// <summary>
        /// Registering new <see cref="IKryptonComponent"/> implemented component(internal variant)
        /// </summary>
        /// <typeparam name="TReq">IKryptonComponent</typeparam>
        /// <param name="self">IKryptonComponent</param>
        /// <returns></returns>
        protected TReq Add<TReq>(TReq self) where TReq : IKryptonComponent, new()
            => StaticCore.Add(self);

        /// <summary>
        /// Delete requested <see cref="IKryptonComponent"/> implemented component(internal variant)
        /// </summary>
        /// <typeparam name="TReq">IKryptonComponent</typeparam>
        /// <returns></returns>
        protected bool Destroy<TReq>() where TReq : IKryptonComponent
            => StaticCore.Destroy<TReq>();

        protected List<IKryptonComponent> GetAll()
            => StaticCore.GetAllComponents();
    }
}
