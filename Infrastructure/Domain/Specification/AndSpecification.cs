﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Specification
{
    public sealed class AndSpecification<T>
       : CompositeSpecification<T>
       where T : class
    {
        private ISpecification<T> _RightSideSpecification = null;
        private ISpecification<T> _LeftSideSpecification = null;
        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == (ISpecification<T>)null)
                throw new ArgumentNullException(nameof(leftSide));

            if (rightSide == (ISpecification<T>)null)
                throw new ArgumentNullException(nameof(rightSide));

            this._LeftSideSpecification = leftSide;
            this._RightSideSpecification = rightSide;
        }
        public override ISpecification<T> LeftSideSpecification
        {
            get { return _LeftSideSpecification; }
        }
        public override ISpecification<T> RightSideSpecification
        {
            get { return _RightSideSpecification; }
        }
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = _LeftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = _RightSideSpecification.SatisfiedBy();

            return (left.And(right));

        }
    }
}
