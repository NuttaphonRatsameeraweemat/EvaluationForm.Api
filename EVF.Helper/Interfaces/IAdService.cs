namespace EVF.Helper.Interfaces
{
    public interface IAdService
    {
        /// <summary>
        /// Connect and Validate username and passowrd from ad service.
        /// </summary>
        /// <param name="username">The string username.</param>
        /// <param name="password">The string password.</param>
        /// <returns></returns>
        bool Authen(string username, string password);
    }
}
