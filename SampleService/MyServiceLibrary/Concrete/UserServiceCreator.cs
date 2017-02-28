using MyServiceLibrary.Abstract;

namespace MyServiceLibrary.Concrete
{
    public class UserServiceCreator : IPoolObjectCreator<UserService, UserServiceMaster>
    {
        public UserService Create()
        {
            return new UserService();
        }

        public UserService Create(UserServiceMaster obj)
        {
            return new UserService(obj);
        }
    }
}
