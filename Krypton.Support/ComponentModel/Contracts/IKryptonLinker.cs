namespace Krypton.Support.ComponentModel.Contracts
{
    public interface IKryptonLinker
    {
        /// <summary>
        /// Registering new inheritance component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        T AddComponent<T>(T self)
            where T : IKryptonComponent;

        /// <summary>
        /// Check on existing requetsed inheritance component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool HasComponent<T>()
            where T : IKryptonComponent;

        /// <summary>
        /// Getting requested inheritance component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetComponent<T>()
            where T : IKryptonComponent;

        /// <summary>
        /// Removeing requested inheritance component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool DestroyComponent<T>()
            where T : IKryptonComponent;
    }
}
