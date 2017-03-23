using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbdulLCTest.Domain;

namespace AbdulLCTest.Business
{
    public interface IPostServices
    {
        PostBDO GetPostById(int postId);
        IEnumerable<PostBDO> GetAllPosts();
        int CreatePost(PostBDO postBDO);
        bool UpdatePost(PostBDO postBDO);
        bool DeletePost(int postId);
    }
}
