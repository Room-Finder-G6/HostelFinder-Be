using FluentValidation;
using HostelFinder.Application.DTOs.Review.Request;

namespace HostelFinder.Application.Validations.Review
{
    public class UpdateReviewRequestValidation : AbstractValidator<UpdateReviewRequestDto>
    {
        public UpdateReviewRequestValidation()
        {
            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Bình luận không được để trống.")
                .MinimumLength(5).WithMessage("Bình luận phải chứa ít nhất 5 kí tự.")
                .MaximumLength(500).WithMessage("Bình luận chứa tối đa 500 kí tự.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Đánh giá phải nằm trong khoảng từ 1 đến 5.");
        }
    }
}
