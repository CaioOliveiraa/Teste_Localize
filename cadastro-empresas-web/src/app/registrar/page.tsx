'use client';

import { useState } from 'react';
import FormInput from '@/components/FormInput';
import Button from '@/components/Button';
import { useRouter } from 'next/navigation';
import api from '@/lib/api'; // ✅ Usa instância com baseURL

export default function RegistrarPage() {
  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const [sucesso, setSucesso] = useState('');
  const router = useRouter();

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setSucesso('');

    try {
      const response = await api.post('/auth/registrar', {
        nome,
        email,
        senha,
      });

      if (response.status === 200) {
        setSucesso('Conta criada com sucesso! Redirecionando...');
        setTimeout(() => router.push('/login'), 1500);
      }
    } catch (error: any) {
      if (error.response?.status === 400) {
        setErro(error.response.data?.mensagem || 'Erro ao registrar.');
      } else {
        setErro('Erro ao registrar. Tente novamente.');
      }
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="w-full max-w-md bg-white p-8 rounded-xl shadow-lg border-t-4 border-[#0057B7]">
        <h1 className="text-2xl font-bold text-[#0057B7] mb-6 text-center">
          Criar Conta
        </h1>

        <form onSubmit={handleRegister} className="space-y-4">
          <FormInput
            label="Nome"
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            required
          />

          <FormInput
            label="Email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <FormInput
            label="Senha"
            type="password"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            required
          />

          {erro && <p className="text-red-600 text-sm text-center">{erro}</p>}
          {sucesso && <p className="text-green-600 text-sm text-center">{sucesso}</p>}

          <Button type="submit">Registrar</Button>
        </form>

        <p className="text-sm mt-4 text-center text-[#0057B7]">
          Já tem uma conta?{' '}
          <button
            onClick={() => router.push('/login')}
            className="font-bold text-[#0057B7] hover:underline transition cursor-pointer"
          >
            Fazer login
          </button>
        </p>
      </div>
    </div>
  );
}
