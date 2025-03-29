using System.Security.Cryptography;

namespace ReizzzToDo.BAL.Services.Utils.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // recommended size is at least 128 bits = 16 bytes
        private const int HashSize = 32; // recommended value is at least 256 bits = 32 bytes
        private const int Iterations = 100000; // number of iteration that hash function is going to run (should use at least 100000 iterations)

        private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512; // hash algorithm the server is going to use to hash password

        public string Hash(string password)
        {

            var salt = RandomNumberGenerator.GetBytes(SaltSize); // generate a random salt value

            // generate hash using Password based key derivation function (Pbkdf2)
            // to produce the password hash in a cryptographically safe way that is resistant to many Brute Force attack
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize); 

            // return hash value by convert byte[] to HexString and include the salt value to create a unique hash
            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public bool Verify(string password, string passwordHashed)
        {
            string[] parts = passwordHashed.Split('-');
            byte[] hash = Convert.FromHexString(parts[0]);
            byte[] salt = Convert.FromHexString(parts[1]);

            // hash the input password in order to know if the passwords are the same
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            //// compare the hash and the input hash
            //// can be used but this opens up an attack Vector called timing attack where malicious actor can use 
            //// the information how long it takes to compare hash and inputHash value to try to figure out what is the correct password hash
            
            // return hash.SequenceEqual(inputHash);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
