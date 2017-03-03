using PublishProfileContracts;
using PublishProfileManager.Properties;

namespace PublishProfileManager.Models
{
    public class UserPublishProfile : PublishProfileBase, IUserPublishProfile
    {
        public string EncryptedPassword { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
