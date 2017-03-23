using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using AbdulLCTest.Domain;
using AbdulLCTest.Data;

namespace AbdulLCTest.Business
{
    public class PostServices : IPostServices
    {

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public PostServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Fetches posts details by id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public PostBDO GetPostById(int posttId)
        {
            var post = _unitOfWork.PostRepository.GetByID(posttId);
            if (post != null)
            {
                Mapper.Initialize(cfg => cfg.CreateMap<Posts, PostBDO>());
                var posttModel = Mapper.Map<Posts, PostBDO>(post);
                return posttModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the posts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PostBDO> GetAllPosts()
        {
            var post = _unitOfWork.PostRepository.GetAll().ToList();
            if (post.Any())
            {
                Mapper.Initialize(cfg => cfg.CreateMap<Posts, PostBDO>());
                var posttModel = Mapper.Map<List<Posts>, List<PostBDO>>(post);
                return posttModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a new post
        /// </summary>
        /// <param name="productEntity"></param>
        /// <returns></returns>
        public int CreatePost(PostBDO postBDO)
        {
            using (var scope = new TransactionScope())
            {
                var post = new Posts
                {
                    Subject = postBDO.Subject,
                    Description = postBDO.Description,
                    CreatedBy = postBDO.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                _unitOfWork.PostRepository.Insert(post);
                _unitOfWork.Save();
                scope.Complete();
                return post.Id;
            }
        }

        /// <summary>
        /// Update a existing post
        /// </summary>
        /// <param name="postBDO"></param>
        /// <returns></returns>
        public bool UpdatePost(PostBDO postBDO)
        {
          var success = false;

          var post = _unitOfWork.PostRepository.GetByID(postBDO.Id);
            if (post != null)
            {
                using (var scope = new TransactionScope())
                {
                    post.Subject = postBDO.Subject;
                    post.Description = postBDO.Description;
                    post.ModifiedBy = postBDO.ModifiedBy;
                    post.ModifiedDate = DateTime.Now;
                    _unitOfWork.PostRepository.Update(post);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;

                }
            }
            else
                success = false;

            return success;
        }

        /// <summary>
        /// Deletes a particular post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public bool DeletePost(int postId)
        {
            var success = false;
            if (postId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var post = _unitOfWork.PostRepository.GetByID(postId);
                    if (post != null)
                    {

                        _unitOfWork.PostRepository.Delete(post);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

    }
}
