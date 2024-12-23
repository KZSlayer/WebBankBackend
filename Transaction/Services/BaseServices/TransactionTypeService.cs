using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Transaction.DTOs;
using Transaction.Models;
using Transaction.Repositories;
using Transaction.Services.Exceptions;

namespace Transaction.Services.BaseServices
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository transactionTypeRepository;
        public TransactionTypeService(ITransactionTypeRepository _transactionTypeRepository)
        {
            transactionTypeRepository = _transactionTypeRepository;
        }
        public async Task CreateTransactionTypeAsync(CreateTransactionTypeDTO transactionTypeDTO)
        {
            var dbTransaction = await transactionTypeRepository.BeginTransactionAsync();
            try
            {
                if (await transactionTypeRepository.CheckIfTransactionTypeExistAsync(transactionTypeDTO.Name))
                {
                    throw new TransactionTypeAlreadyExistException();
                }
                var newTransactionType = new TransactionType
                {
                    Name = transactionTypeDTO.Name,
                    Description = transactionTypeDTO.Description,
                };
                await transactionTypeRepository.AddTransactionTypeAsync(newTransactionType);
                await dbTransaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
            catch (OperationCanceledException)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
            catch (TransactionTypeAlreadyExistException)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
        public async Task UpdateTransactionTypeAsync(ChangeTransactionTypeDTO changeTransactionTypeDTO)
        {
            var dbTransaction = await transactionTypeRepository.BeginTransactionAsync();
            try
            {
                var existTransactionType = await transactionTypeRepository.GetTransactionTypeByNameAsync(changeTransactionTypeDTO.ExistName);
                if (existTransactionType == null)
                {
                    throw new TransactionTypeNotFoundException();
                }
                existTransactionType.Name = changeTransactionTypeDTO.NewName;
                existTransactionType.Description = changeTransactionTypeDTO.NewDescription;
                await transactionTypeRepository.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
            catch (OperationCanceledException)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
            catch (TransactionTypeNotFoundException)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
