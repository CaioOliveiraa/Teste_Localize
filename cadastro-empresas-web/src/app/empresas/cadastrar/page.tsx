'use client';

import { useState } from 'react';
import FormInput from '@/components/FormInput';
import Button from '@/components/Button';
import { useRouter } from 'next/navigation';
import api from '@/lib/api';

export default function CadastrarEmpresaPage() {
  const [cnpj, setCnpj] = useState('');
  const [erro, setErro] = useState('');
  const [sucesso, setSucesso] = useState('');
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setSucesso('');
    setLoading(true);

    try {
      await api.post(
        '/empresas',
        { cnpj },
        { withCredentials: true }
      );

      setSucesso('Empresa cadastrada com sucesso!');
      setCnpj('');
    } catch {
      setErro('Erro ao cadastrar empresa. Verifique o CNPJ ou tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="w-full max-w-md bg-white p-8 rounded-xl shadow-xl border-t-4 border-[#0057B7]">
        <button
          onClick={() => router.push('/empresas')}
          className="text-sm text-[#0057B7] hover:underline mb-4 cursor-pointer transition"
        >
          ← Voltar para a listagem
        </button>

        <h1 className="text-2xl font-bold text-[#0057B7] mb-6">
          Cadastrar Empresa
        </h1>

        <form onSubmit={handleSubmit} className="space-y-4">
          <FormInput
            label="CNPJ"
            type="text"
            value={cnpj}
            onChange={e => setCnpj(e.target.value)}
            required
          />

          {erro && <p className="text-red-600 text-sm">{erro}</p>}
          {sucesso && <p className="text-green-600 text-sm">{sucesso}</p>}

          <Button type="submit" disabled={loading}>
            {loading ? 'Cadastrando...' : 'Cadastrar'}
          </Button>
        </form>
      </div>
    </div>
  );
}
