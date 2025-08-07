"use client"
import { LoginForm } from '@/components/auth/login-form';
import Link from 'next/link';
import { TypingAnimation } from '@/components/magicui/typing-animation';

export default function LoginPage() {
  return (
    <div className="min-h-screen flex flex-col px-4">
      <div className="pt-8 text-center">
        <TypingAnimation 
          className="text-3xl font-bold text-gray-800"
          as="h1"
          duration={50}
        >
            CHAT APP MINI
        </TypingAnimation>
      </div>

      <div className="flex flex-1 items-center justify-center flex-col">
        <LoginForm />
        <p className="mt-6 text-sm text-center text-gray-600">
          Don&apos;t have an account?{' '}
          <Link href="/register" className="text-blue-600 hover:underline">
            Register here
          </Link>
        </p>
      </div>
    </div>
  );
}
