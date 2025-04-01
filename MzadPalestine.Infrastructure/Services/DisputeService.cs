using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MzadPalestine.Core.DTOs.Disputes;
using MzadPalestine.Core.Interfaces;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;
using System.Collections.Generic;

namespace MzadPalestine.Infrastructure.Services;

public class DisputeService : IDisputeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DisputeService(IUnitOfWork unitOfWork , IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DisputeDto>> CreateDisputeAsync(string userId , CreateDisputeDto disputeDto)
    {
        var dispute = _mapper.Map<Dispute>(disputeDto);
        dispute.UserId = userId;
        dispute.CreatedAt = DateTime.UtcNow;

        await _unitOfWork.Disputes.AddAsync(dispute);
        await _unitOfWork.SaveChangesAsync();

        var disputeResult = _mapper.Map<DisputeDto>(dispute);
        return Result<DisputeDto>.CreateSuccess(disputeResult);
    }

    public async Task<Result<DisputeDto>> GetDisputeByIdAsync(string userId , int disputeId)
    {
        var dispute = await _unitOfWork.Disputes.GetByIdAsync(disputeId);
        if (dispute == null)
            return Result<DisputeDto>.CreateFailure("Dispute not found");

        if (dispute.UserId != userId)
            return Result<DisputeDto>.CreateFailure("Unauthorized access to dispute");

        var disputeDto = _mapper.Map<DisputeDto>(dispute);
        return Result<DisputeDto>.CreateSuccess(disputeDto);
    }

    public async Task<PagedResult<DisputeDto>> GetUserDisputesAsync(string userId , PaginationParams parameters)
    {
        var disputes = await _unitOfWork.Disputes.GetUserDisputesAsync(userId , parameters);
        var disputeDtos = _mapper.Map<List<DisputeDto>>(disputes);

        return new PagedResult<DisputeDto>
        {
            Items = disputeDtos ,
            PageNumber = parameters.PageNumber ,
            PageSize = parameters.PageSize ,
            TotalCount = disputes.TotalCount ,
            TotalPages = disputes.TotalPages
        };
    }

    public async Task<Result<DisputeDto>> ResolveDisputeAsync(string adminId , int disputeId , ResolveDisputeDto resolveDto)
    {
        var dispute = await _unitOfWork.Disputes.GetByIdAsync(disputeId);
        if (dispute == null)
            return Result<DisputeDto>.CreateFailure("Dispute not found");

        dispute.IsResolved = true;
        dispute.Status = DisputeStatus.Resolved;
        dispute.ResolvedById = adminId;

        await _unitOfWork.SaveChangesAsync();

        var disputeDto = _mapper.Map<DisputeDto>(dispute);
        return Result<DisputeDto>.CreateSuccess(disputeDto);
    }

    public async Task<Result<DisputeEvidenceDto>> AddDisputeEvidenceAsync(string userId , int disputeId , AddDisputeEvidenceRequestDto evidenceDto)
    {
        var dispute = await _unitOfWork.Disputes.GetByIdAsync(disputeId);
        if (dispute == null)
            return Result<DisputeEvidenceDto>.CreateFailure("Dispute not found");

        if (dispute.UserId != userId)
            return Result<DisputeEvidenceDto>.CreateFailure("Unauthorized access to dispute");

        var evidence = new DisputeEvidence
        {
            DisputeId = disputeId ,
            Description = evidenceDto.Description ,
            FileUrl = string.Empty, // Will be set after file upload
            FileType = evidenceDto.EvidenceType,
            SubmittedById = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DisputeEvidences.AddAsync(evidence);
        await _unitOfWork.SaveChangesAsync();

        var evidenceResult = _mapper.Map<DisputeEvidenceDto>(evidence);
        return Result<DisputeEvidenceDto>.CreateSuccess(evidenceResult);
    }

    public async Task<PagedResult<DisputeDto>> GetAuctionDisputesAsync(int auctionId , PaginationParams parameters)
    {
        var disputes = await _unitOfWork.Disputes.GetDisputesByAuctionAsync(auctionId);
        var disputeDtos = _mapper.Map<List<DisputeDto>>(disputes);

        var totalCount = disputeDtos.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize);
        var items = disputeDtos
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        return new PagedResult<DisputeDto>
        {
            Items = items ,
            PageNumber = parameters.PageNumber ,
            PageSize = parameters.PageSize ,
            TotalCount = totalCount ,
            TotalPages = totalPages
        };
    }
}