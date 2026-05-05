import { useState } from 'react';
import { useFormValidation } from '../hooks/useFormValidation';
import './RegistrationForm.css';

interface PatientFormValues {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  phoneNumber: string;
  email: string;
}

const initialValues: PatientFormValues = {
  firstName: '',
  lastName: '',
  dateOfBirth: '',
  phoneNumber: '',
  email: '',
};

const validationRules = {
  firstName: (v: string) => !v.trim() ? 'First name is required.' : null,
  lastName: (v: string) => !v.trim() ? 'Last name is required.' : null,
  dateOfBirth: (v: string) => {
    if (!v) return 'Date of birth is required.';
    const dob = new Date(v);
    const today = new Date();
    if (dob >= today) return 'Date of birth must be in the past.';
    const minDate = new Date();
    minDate.setFullYear(minDate.getFullYear() - 130);
    if (dob < minDate) return 'Date of birth cannot be more than 130 years ago.';
    return null;
  },
  phoneNumber: (v: string) => {
    if (!v.trim()) return 'Phone number is required.';
    const phoneRegex = /^\d{3}-\d{4}$|^\d{3}-\d{3}-\d{4}$|^\(\d{3}\)\s\d{3}-\d{4}$/;
    if (!phoneRegex.test(v)) return 'Format: 555-1234 or 555-123-4567';
    return null;
  },
  email: (v: string) => {
    if (!v.trim()) return 'Email is required.';
    if (!/\S+@\S+\.\S+/.test(v)) return 'Enter a valid email address.';
    return null;
  },
};

type SubmitStatus = 'idle' | 'loading' | 'success' | 'error';

export default function RegistrationForm() {
  const { values, errors, touched, handleChange, handleBlur, validateAll, reset } =
    useFormValidation(initialValues, validationRules);

  const [submitStatus, setSubmitStatus] = useState<SubmitStatus>('idle');
  const [apiError, setApiError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateAll()) return;

    setSubmitStatus('loading');
    setApiError(null);

    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/api/patient`,  {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          firstName: values.firstName,
          lastName: values.lastName,
          dateOfBirth: new Date(values.dateOfBirth).toISOString(),
          phoneNumber: values.phoneNumber,
          email: values.email,
        }),
      });

      if (response.status === 201) {
        setSubmitStatus('success');
        reset();
      } else {
        const data = await response.json();
        setApiError(data.error || 'Something went wrong. Please try again.');
        setSubmitStatus('error');
      }
    } catch (err) {
      setApiError('Unable to reach the server. Please try again later.');
      setSubmitStatus('error');
    }
  };

  if (submitStatus === 'success') {
    return (
      <div className="success-banner">
        <h2>✓ Patient registered successfully</h2>
        <button className="btn-secondary" onClick={() => setSubmitStatus('idle')}>
          Register another patient
        </button>
      </div>
    );
  }

  return (
    <form className="registration-form" onSubmit={handleSubmit} noValidate>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="firstName">First Name *</label>
          <input
            id="firstName"
            type="text"
            value={values.firstName}
            onChange={e => handleChange('firstName', e.target.value)}
            onBlur={() => handleBlur('firstName')}
            className={touched.firstName && errors.firstName ? 'input-error' : ''}
          />
          {touched.firstName && errors.firstName && (
            <span className="error-msg">{errors.firstName}</span>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="lastName">Last Name *</label>
          <input
            id="lastName"
            type="text"
            value={values.lastName}
            onChange={e => handleChange('lastName', e.target.value)}
            onBlur={() => handleBlur('lastName')}
            className={touched.lastName && errors.lastName ? 'input-error' : ''}
          />
          {touched.lastName && errors.lastName && (
            <span className="error-msg">{errors.lastName}</span>
          )}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="dateOfBirth">Date of Birth *</label>
        <input
          id="dateOfBirth"
          type="date"
          value={values.dateOfBirth}
          onChange={e => handleChange('dateOfBirth', e.target.value)}
          onBlur={() => handleBlur('dateOfBirth')}
          className={touched.dateOfBirth && errors.dateOfBirth ? 'input-error' : ''}
        />
        {touched.dateOfBirth && errors.dateOfBirth && (
          <span className="error-msg">{errors.dateOfBirth}</span>
        )}
      </div>

      <div className="form-group">
        <label htmlFor="phoneNumber">Phone Number *</label>
        <input
          id="phoneNumber"
          type="tel"
          placeholder="555-123-4567"
          value={values.phoneNumber}
          onChange={e => handleChange('phoneNumber', e.target.value)}
          onBlur={() => handleBlur('phoneNumber')}
          className={touched.phoneNumber && errors.phoneNumber ? 'input-error' : ''}
        />
        {touched.phoneNumber && errors.phoneNumber && (
          <span className="error-msg">{errors.phoneNumber}</span>
        )}
      </div>

      <div className="form-group">
        <label htmlFor="email">Email *</label>
        <input
          id="email"
          type="email"
          value={values.email}
          onChange={e => handleChange('email', e.target.value)}
          onBlur={() => handleBlur('email')}
          className={touched.email && errors.email ? 'input-error' : ''}
        />
        {touched.email && errors.email && (
          <span className="error-msg">{errors.email}</span>
        )}
      </div>

      {apiError && (
        <div className="api-error">{apiError}</div>
      )}

        <button
            type="submit"
            className="btn-primary"
            disabled={submitStatus === 'loading'}
            aria-busy={submitStatus === 'loading'}
             >
        {submitStatus === 'loading' ? 'Registering...' : 'Register Patient'}
      </button>

    </form>
  );
}