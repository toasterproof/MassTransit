﻿// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.RabbitMqTransport.Contexts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Context;
    using Integration;
    using RabbitMQ.Client;


    public class SharedModelContext :
        ModelContext
    {
        readonly CancellationToken _cancellationToken;
        readonly ModelContext _context;

        public SharedModelContext(ModelContext context, CancellationToken cancellationToken)
        {
            _context = context;
            _cancellationToken = cancellationToken;
        }

        bool PipeContext.HasPayloadType(Type contextType)
        {
            return _context.HasPayloadType(contextType);
        }

        bool PipeContext.TryGetPayload<TPayload>(out TPayload payload)
        {
            return _context.TryGetPayload(out payload);
        }

        TPayload PipeContext.GetOrAddPayload<TPayload>(PayloadFactory<TPayload> payloadFactory)
        {
            return _context.GetOrAddPayload(payloadFactory);
        }

        ConnectionContext ModelContext.ConnectionContext
        {
            get { return _context.ConnectionContext; }
        }

        Task ModelContext.BasicPublishAsync(string exchange, string routingKey, bool mandatory, bool immediate, IBasicProperties basicProperties, byte[] body)
        {
            return _context.BasicPublishAsync(exchange, routingKey, mandatory, immediate, basicProperties, body);
        }

        IModel ModelContext.Model
        {
            get { return _context.Model; }
        }

        CancellationToken PipeContext.CancellationToken
        {
            get { return _cancellationToken; }
        }
    }
}