using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wyvern.Application.DTOs.Item;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Repositories;
namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ItemController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemResponseDto>> GetItens()
        {
            var itens = _uof.ItemRepository.GetItens();
            if (!itens.Any())
            {
                return NotFound("Item não encontrado");
            }
            var itensDto = _mapper.Map<List<ItemResponseDto>>(itens);
            return Ok(itensDto);
        }
        [HttpGet("{id:int}")]
        public ActionResult <ItemResponseDto> GetItemById(int id)
        {
            var item = _uof.ItemRepository.GetItem(id);
            if( item == null)
            {
                return NotFound("Item nao encontrado");
            }
            var itemDto = _mapper.Map<ItemResponseDto>(item);
            return Ok(itemDto);
        }
        [HttpPost]
        public ActionResult<ItemResponseDto> CreateItem(CreateItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("item inválido");
            }
            var item = _mapper.Map<Item>(itemDto);
            _uof.ItemRepository.CreateItem(item);
            var itemCriadoDto = _mapper.Map<ItemResponseDto>(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.ItemId }, itemCriadoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateItem(int id, ItemUpdateDto itemDto)
        {
            if (itemDto == null)
                return BadRequest("Dados inválidos");
            var itemNoBanco = _uof.ItemRepository.GetItem(id);
            if (itemNoBanco == null)
            {
                return BadRequest("Id do item não correspondente à rota");
            }
            _mapper.Map(itemDto, itemNoBanco);
            _uof.ItemRepository.UpdateItem(itemNoBanco);
            var itemAtualizado = _uof.ItemRepository.GetItem(id);
            var itemDtoAtualizado = _mapper.Map<ItemResponseDto>(itemAtualizado);
            return Ok(_mapper.Map<ItemResponseDto>(itemNoBanco));
            
        }
        [HttpDelete("{id:int}")]
        public ActionResult DeleteItem(int id)
        {
            var item = _uof.ItemRepository.DeleteItem(id);
            if( item == null)
            {
                return NotFound("item nao encontrado");

            }
            return Ok("item deletado com sucesso");
        }


    }
}
