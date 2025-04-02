using Server.Models.DTOs;

namespace Server.Repository.IRepo
{
    public interface IReviewRepo
    {
        Task<List<selfReviewListDTO>> selfReviewList(string token, string? search, int? statusId);
        Task<selfReviewDTO> getSelfReview(int id);
        Task<List<managerReviewList>> managerReviewList(string token, string? search, int? rating);
        Task<bool> selfReview(string token, selfReviewDTO selfReview);
        Task<bool> managerReview(string token, managerReviewDTO managerReview);
        Task<bool> managerRevise(string token, int id, string managerFeedback);
        Task<bool> editSelfReview(int id, selfReviewDTO selfReview);
        Task<bool> deleteSelfReview(int id);
    }
}
