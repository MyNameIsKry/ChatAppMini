"use client"
import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import axios from 'axios';
import { 
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { InteractiveHoverButton } from "@/components/magicui/interactive-hover-button";

const registerSchema = z.object({
  email: z.string().email('Invalid email format'),
  username: z.string().min(3, 'Username must be at least 3 characters'),
  password: z.string().min(6, 'Password must be at least 6 characters'),
  confirmPassword: z.string()
}).refine((data) => data.password === data.confirmPassword, {
  message: "Passwords don't match",
  path: ["confirmPassword"],
});

type RegisterFormData = z.infer<typeof registerSchema>;

export function RegisterForm() {
  const router = useRouter();
  const [error, setError] = useState('');
  
  const form = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      email: '',
      username: '',
      password: '',
      confirmPassword: '',
    },
  });

  const onSubmit = async (data: RegisterFormData) => {
    try {
      const res = await axios.post('http://localhost:5189/api/auth/register', {
        email: data.email,
        name: data.username,
        password: data.password,
        confirmPassword: data.confirmPassword,
      });

      if (res.data.status === 400) {
        setError(res.data.message || 'Registration failed');
        return;
      }

      router.push('/login');
    } catch (err: any) {
      setError(err.response?.data?.message || 'An error occurred during registration');
    }
  };

  return (
    <div className="w-full max-w-md p-6 space-y-4 bg-white rounded-lg shadow-lg">
      <h2 className="text-2xl font-bold text-center">Register</h2>
      {error && <div className="p-3 text-sm text-red-500 bg-red-50 rounded">{error}</div>}
      
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
          <FormField
            control={form.control}
            name="email"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Email</FormLabel>
                <FormControl>
                  <Input
                    type="email"
                    placeholder="Enter your email"
                    {...field}
                    className="w-full"
                  />
                </FormControl>
                <FormMessage>
                  {form.formState.errors.email && <span className="text-red-500">{form.formState.errors.email.message}</span>}
                </FormMessage>
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="username"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Username</FormLabel>
                <FormControl>
                  <Input
                    type="text"
                    placeholder="Enter your username"
                    {...field}
                    className="w-full"
                  />
                </FormControl>
                <FormMessage>
                  {form.formState.errors.username && <span className="text-red-500">{form.formState.errors.username.message}</span>}
                </FormMessage>
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="password"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Password</FormLabel>
                <FormControl>
                  <Input
                    type="password"
                    placeholder="Enter your password"
                    {...field}
                    className="w-full"
                  />
                </FormControl>
                <FormMessage>
                  {form.formState.errors.password && <span className="text-red-500">{form.formState.errors.password.message}</span>}
                </FormMessage>
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="confirmPassword"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Confirm Password</FormLabel>
                <FormControl>
                  <Input
                    type="password"
                    placeholder="Confirm your password"
                    {...field}
                    className="w-full"
                  />
                </FormControl>
                <FormMessage>
                  {form.formState.errors.confirmPassword && <span className="text-red-500">{form.formState.errors.confirmPassword.message}</span>}
                </FormMessage>
              </FormItem>
            )}
          />
          <InteractiveHoverButton
            type="submit"
            disabled={form.formState.isSubmitting}
          >
            {form.formState.isSubmitting ? 'Registering...' : 'Register'}
          </InteractiveHoverButton>
        </form>
      </Form>
    </div>
  );
}
