'use client';

import { useState } from 'react';
import FormInput from '@/components/FormInput';
import Button from '@/components/Button';
import { useRouter } from 'next/navigation';
import axios from 'axios';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');
  const router = useRouter();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');

    try {
      const response = await axios.post(
        'http://localhost:5114/auth/login',
        { email, senha },
        { withCredentials: true }
      );

      if (response.status === 200) {
        router.push('/empresas');
      }
    } catch {
      setErro('Email ou senha inválidos.');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="w-full max-w-md bg-white p-8 rounded-xl shadow-lg border-t-4 border-[#0057B7]">
        <h1 className="text-2xl font-bold text-[#0057B7] mb-6 text-center">
          Acesso à Plataforma
        </h1>

        <form onSubmit={handleLogin} className="space-y-4">
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

          <Button type="submit">Entrar</Button>
        </form>

        <p className="text-sm mt-4 text-center text-[#0057B7]">
          Não tem uma conta?{' '}
          <button
            onClick={() => router.push('/registrar')}
            className="font-bold text-[#0057B7] hover:underline transition cursor-pointer"
          >
            Criar conta
          </button>
        </p>
      </div>
    </div>
  );
}
