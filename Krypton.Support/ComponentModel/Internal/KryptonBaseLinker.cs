using Krypton.Support.ComponentModel.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Krypton.Support.ComponentModel.Internal
{
    internal class KryptonBaseLinker : ComponentsMap, IKryptonLinker
    {
        /// <summary>
        /// Added <see cref="IKryptonComponent"/> modules.
        /// </summary>
        private static List<IKryptonComponent> Modules = new List<IKryptonComponent>();

        public KryptonBaseLinker()
        {
            for (int i = 0; i < Map.Count; i++)
            {
                AddComponent(Map[i]);
            }
        }

        /// <summary>
        /// Returns registered <see cref="IKryptonComponent"/> implemented component
        /// </summary>
        /// <typeparam name="T">Компонент для поиска</typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : IKryptonComponent
        {
            if (HasComponent<T>())
            {
                return (T)Modules.First((x) => x.GetType() == typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// Registering new <see cref="IKryptonComponent"/> implemented component
        /// </summary>
        /// <typeparam name="T">IKryptonComponent</typeparam>
        /// <param name="self">IKryptonComponent</param>
        /// <returns></returns>
        public T AddComponent<T>(T self) where T : IKryptonComponent
        {
            if (!HasComponent<T>())
            {
                Modules.Add(self);
            }

            return GetComponent<T>();
        }

        /// <summary>
        /// Deleting the <see cref="IKryptonComponent"/> implemented component
        /// </summary>
        /// <typeparam name="T">IKryptonComponent</typeparam>
        /// <returns></returns>
        public bool DestroyComponent<T>() where T : IKryptonComponent
        {
            if (HasComponent<T>())
            {
                Modules.Remove(GetComponent<T>());

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns logic value about exists <see cref="IKryptonComponent"/> implemented component
        /// </summary>
        /// <typeparam name="T">IKryptonComponent</typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : IKryptonComponent
            => Modules.Any((x) => x.GetType() == typeof(T));

        public List<IKryptonComponent> GetAllComponents()
            => Modules;
    }
}
