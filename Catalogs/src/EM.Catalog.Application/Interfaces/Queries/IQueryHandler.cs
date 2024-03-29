﻿using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{ }
