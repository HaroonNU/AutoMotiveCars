namespace AutoMotiveProject.cs.Services
{
    public class AuthService
    {
        private readonly string _credentialsFile = "credentials.txt";

        public bool ValidateUser(string email, string password)
        {
            if (!File.Exists(_credentialsFile)) return false;

            var lines = File.ReadAllLines(_credentialsFile);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && parts[0] == email && parts[1] == password)
                    return true;
            }
            return false;
        }

        public bool UpdatePassword(string email, string newPassword)
        {
            if (!File.Exists(_credentialsFile)) return false;

            var lines = File.ReadAllLines(_credentialsFile);
            var updated = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split('|');
                if (parts.Length == 2 && parts[0] == email)
                {
                    lines[i] = $"{email}|{newPassword}";
                    updated = true;
                    break;
                }
            }

            if (updated)
            {
                File.WriteAllLines(_credentialsFile, lines);
            }

            return updated;
        }

        public bool UserExists(string email)
        {
            if (!File.Exists(_credentialsFile)) return false;

            var lines = File.ReadAllLines(_credentialsFile);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2 && parts[0] == email)
                    return true;
            }
            return false;
        }
    }
}