using FelixHelper.Abstract;
using Microsoft.Extensions.Logging;
using System;

namespace FelixHelper.Services;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);
