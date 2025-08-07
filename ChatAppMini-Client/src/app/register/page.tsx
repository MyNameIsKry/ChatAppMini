import { RegisterForm } from '@/components/auth/register-form';
import { TypingAnimation } from '@/components/magicui/typing-animation';
import Link from 'next/link';

export default function RegisterPage() {
  return (
    <div className="min-h-screen bg-gray-50 flex flex-col px-4">
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
        <RegisterForm />
        <p className="mt-6 text-sm text-center text-gray-600">
          Already have an account?{' '}
          <Link href="/login" className="text-blue-600 hover:underline">
            Login here
          </Link>
          </p>
      </div>
    </div>
  );
}
