using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
            {
                return NotFound(); // Retorna 404 Not Found se a tarefa não for encontrada
            }

            return Ok(tarefa); // Retorna 200 OK com a tarefa encontrada
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList(); // Obtém todas as tarefas do banco

            return Ok(tarefas); // Retorna 200 OK com a lista de tarefas
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefas = _context.Tarefas
                                .Where(t => t.Titulo.Contains(titulo))
                                .ToList();

            return Ok(tarefas); // Retorna 200 OK com as tarefas filtradas pelo título
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefas = _context.Tarefas
                                .Where(t => t.Data.Date == data.Date)
                                .ToList();

            return Ok(tarefas); // Retorna 200 OK com as tarefas filtradas pela data
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefas = _context.Tarefas
                                .Where(t => t.Status == status)
                                .ToList();

            return Ok(tarefas); // Retorna 200 OK com as tarefas filtradas pelo status
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            _context.Tarefas.Add(tarefa); // Adiciona a nova tarefa ao contexto
            _context.SaveChanges(); // Salva as mudanças no banco

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa); // Retorna 201 Created com a nova tarefa
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound(); // Retorna 404 Not Found se a tarefa não for encontrada
            }

            if (tarefa.Data == DateTime.MinValue)
            {
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            }

            // Atualiza as informações da tarefa existente com base na nova tarefa
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            _context.SaveChanges(); // Salva as mudanças no banco

            return Ok(); // Retorna 200 OK após a atualização
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
            {
                return NotFound(); // Retorna 404 Not Found se a tarefa não for encontrada
            }

            _context.Tarefas.Remove(tarefaBanco); // Remove a tarefa do contexto
            _context.SaveChanges(); // Salva as mudanças no banco

            return NoContent(); // Retorna 204 No Content após a remoção
        }
    }
}
