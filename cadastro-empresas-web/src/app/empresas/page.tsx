'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import api from '@/lib/api';
import Link from 'next/link';
import { removerToken } from '@/lib/auth';


interface Empresa {
  nomeFantasia: string;
  nomeEmpresarial: string;
  cnpj: string;
  situacao: string;
  abertura: string;
  tipo: string;
  naturezaJuridica: string;
  atividadePrincipal: string;
  logradouro: string;
  numero: string;
  complemento: string;
  bairro: string;
  municipio: string;
  uf: string;
  cep: string;
}


export default function EmpresasPage() {
  const [empresas, setEmpresas] = useState<Empresa[]>([]);
  const [erro, setErro] = useState('');
  const router = useRouter();

  useEffect(() => {
    const buscarEmpresas = async () => {
      try {
        const response = await api.get<Empresa[]>('/empresas', {
          params: { pagina: 1, quantidade: 10 },
          withCredentials: true,
        });
        setEmpresas(response.data);
      } catch (error) {
        setErro('Erro ao buscar empresas. Faça login novamente.');
        router.push('/login');
      }
    };

    buscarEmpresas();
  }, [router]);

  const handleLogout = () => {
    removerToken();
    router.push('/login');
  };

  function formatarCnpj(cnpj: string) {
    return cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
  }

  return (
    <div className="min-h-screen bg-[#F8F9FC] px-6 py-10">
      <div className="max-w-5xl mx-auto">
        <div className="flex items-center justify-between mb-8">
          <h1 className="text-3xl font-bold text-[#0057B7]">Minhas Empresas</h1>
          <button
            onClick={handleLogout}
            className="bg-[#E63946] text-white px-4 py-2 rounded-md hover:bg-[#d62828] transition"
          >
            Sair
          </button>
        </div>

        <div className="flex justify-start mb-6">
          <Link href="/empresas/cadastrar">
            <button className="bg-[#0057B7] text-white px-5 py-2 rounded-md shadow hover:bg-[#003f91] transition">
              + Cadastrar nova empresa
            </button>
          </Link>
        </div>

        {erro && <p className="text-red-600 text-center mb-6 font-medium">{erro}</p>}

        {empresas.length === 0 && !erro ? (
          <p className="text-gray-600 text-center">Você ainda não cadastrou nenhuma empresa.</p>
        ) : (
          <ul className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {empresas.map((empresa, index) => (
              <li
                key={index}
                className="bg-white p-5 rounded-lg shadow hover:shadow-md border-l-4 border-[#0057B7] transition"
              >
                <p className="text-xl font-semibold text-[#0057B7]">
                  {empresa.nomeFantasia || empresa.nomeEmpresarial}
                </p>
                <p className="text-sm text-gray-600">CNPJ: {formatarCnpj(empresa.cnpj)}</p>
                <p className="text-sm text-gray-600">Situação: {empresa.situacao}</p>
                <p className="text-sm text-gray-600">Tipo: {empresa.tipo}</p>
                <p className="text-sm text-gray-600">Natureza Jurídica: {empresa.naturezaJuridica}</p>
                <p className="text-sm text-gray-600">Atividade Principal: {empresa.atividadePrincipal}</p>
                <p className="text-sm text-gray-600">Abertura: {empresa.abertura}</p>
                <p className="text-sm text-gray-600">
                  Endereço: {empresa.logradouro}, {empresa.numero}
                  {empresa.complemento && ` - ${empresa.complemento}`} - {empresa.bairro}, {empresa.municipio} - {empresa.uf}, {empresa.cep}
                </p>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}
