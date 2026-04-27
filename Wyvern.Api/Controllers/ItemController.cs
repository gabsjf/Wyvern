using Microsoft.AspNetCore.Mvc;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;
using AutoMapper;
using Wyvern.Application.DTOs.Item;
namespace Wyvern.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly WyvernDbContext _contexto;
        private readonly IMapper _mapper;
        public ItemController(WyvernDbContext contexto, IMapper mapper)
        {
            _contexto = contexto;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemResponseDto>> GetItens()
        {
            var itens = _contexto.Itens.Where(i => i.Ativo).ToList();
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
            var item = _contexto.Itens.FirstOrDefault(i => i.ItemId == id && i.Ativo);
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
            _contexto.Itens.Add(item);
            _contexto.SaveChanges();
            // o create retorna pra variavel um map do tipo response
            var itemCriadoDto = _mapper.Map<ItemResponseDto>(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.ItemId }, itemCriadoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateItem(int id, ItemUpdateDto itemDto)
        {
            var itemNoBanco = _contexto.Itens.FirstOrDefault(i => i.ItemId == id && i.Ativo);
            if (itemNoBanco == null)
            {
                return BadRequest("Id do item não correspondente à rota");
            }
            _mapper.Map(itemDto, itemNoBanco);
            _contexto.SaveChanges();
            return Ok(_mapper.Map<ItemResponseDto>(itemNoBanco));
            
        }
        [HttpDelete("{id:int}")]
        public ActionResult DeleteItem(int id)
        {
            var item = _contexto.Itens.FirstOrDefault(i => i.ItemId == id);
            if( item == null)
            {
                return NotFound("item nao encontrado");

            }
            item.Ativo = false;
            _contexto.SaveChanges();
            return Ok("item deletado com sucesso");
        }


    }
}
