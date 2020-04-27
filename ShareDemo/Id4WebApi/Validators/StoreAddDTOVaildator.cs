using FluentValidation;
using Id4WebApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Id4WebApi.Validators
{
    public class StoreAddDTOVaildator : AbstractValidator<StoreAddDTO>
    {
        public StoreAddDTOVaildator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.Name).NotNull().Must(StoreNameValid).WithName("酒店名");
            RuleFor(x => x.Address).Length(5, 200).WithMessage("地址不能为空且长度必须符合规则");
            RuleFor(x => x.Star).Must(StoreStarValid).WithMessage("星级必须为1到5星级");
        }

        private bool StoreNameValid(StoreAddDTO dto, string name)
        {
            return name != "admin";
        }

        private bool StoreStarValid(StoreAddDTO dto, int star) 
        {
            return star > 0 && star <= 5;
        }
    }
}
