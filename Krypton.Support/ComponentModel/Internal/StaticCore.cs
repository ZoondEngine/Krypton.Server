using Krypton.Support.ComponentModel.Contracts;
using System.Collections.Generic;

namespace Krypton.Support.ComponentModel.Internal
{
    public static class StaticCore
    {
        private static KryptonBaseLinker Linker = new KryptonBaseLinker();

        /// <summary>
        /// Returns request <see cref="T"/> component(static variant)
        /// </summary>
        /// <typeparam name="T">IKryptonComponent</typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : IKryptonComponent, new()
        {
            if (!Exists<T>())
                Add(new T());

            return Linker.GetComponent<T>();
        }

        /// <summary>
        /// Add <see cref="T"/> component to linker(static variant)
        /// </summary>
        /// <typeparam name="T">IKryptonComponent</typeparam>
        /// <param name="self">IKryptonComponent</param>
        /// <returns></returns>
        public static T Add<T>(T self) where T : IKryptonComponent, new()
        {
            if (!Exists<T>())
                return Linker.AddComponent(self);

            return (T)(object)null;
        }

        /// <summary>
        /// Destroying <see cref="T"/> component from linker(static variant)
        /// </summary>
        /// <typeparam name="T">IKryptonComponent</typeparam>
        /// <returns></returns>
        public static bool Destroy<T>() where T : IKryptonComponent
            => Linker.DestroyComponent<T>();


        /// <summary>
        /// Check module existing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Exists<T>() where T : IKryptonComponent
            => Linker.HasComponent<T>();

        public static List<IKryptonComponent> GetAllComponents()
            => Linker.GetAllComponents();
    }
}
